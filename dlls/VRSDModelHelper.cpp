
#include <string>
#include <filesystem>
#include <regex>
#include <memory>
#include <unordered_map>

#include "extdll.h"
#include "util.h"
#include "cbase.h"

// Intercept model functions to allow usage of SD versions

inline std::filesystem::path GetPathFor(const std::string& file)
{
	std::filesystem::path relativePath = UTIL_GetGameDir() + file;
	return std::filesystem::absolute(relativePath);
}

namespace
{
	std::unordered_map<std::string, std::shared_ptr<char>> gModelNames;
}

char *GetSafeModelCharPtr(const std::string& model)
{
	if (gModelNames.count(model) == 0)
	{
		std::shared_ptr<char> modelPtr(new char[model.size() + 1], [](char* s) { delete[] s; });
		std::copy(model.begin(), model.end(), modelPtr.get());
		modelPtr.get()[model.size()] = 0;
		gModelNames[model] = modelPtr;
	}
	return gModelNames[model].get();
}

char* GetSDModel(const std::string& model)
{
	if (model.find("models/") != 0)
		return nullptr;

	std::string sdModel = std::regex_replace(model, std::regex{"models/"}, "models/SD/");
	std::filesystem::path sdModelPathRelative{ UTIL_GetGameDir() + "/" + sdModel };

	std::filesystem::path sdModelPath = std::filesystem::absolute(sdModelPathRelative);

	if (std::filesystem::exists(sdModelPath))
	{
		return GetSafeModelCharPtr(sdModel);
	}
	else
		return nullptr;
}

const char* GetHDModel(const char* model)
{
	std::string sdModel{ model };

	if (sdModel.find("models/SD/") != 0)
		return model;

	std::string hdModel = std::regex_replace(sdModel, std::regex{ "models/SD/" }, "models/");
	return GetSafeModelCharPtr(hdModel);
}

int PRECACHE_MODEL(char* s)
{
	int sdModelIndex = -1;
	int hdModelIndex = PRECACHE_MODEL2(s);

	char* sdModel = GetSDModel(s);
	if (sdModel != nullptr)
	{
		sdModelIndex = PRECACHE_MODEL2(sdModel);
	}

	if (sdModelIndex >= 0 && CVAR_GET_FLOAT("vr_use_sd_models") != 0.f)
		return sdModelIndex;

	return hdModelIndex;
}

int PRECACHE_GENERIC(char* s)
{
	int sdModelIndex = -1;
	int hdModelIndex = PRECACHE_GENERIC2(s);

	char* sdModel = GetSDModel(s);
	if (sdModel != nullptr)
	{
		sdModelIndex = PRECACHE_GENERIC2(sdModel);
	}

	if (sdModelIndex >= 0 && CVAR_GET_FLOAT("vr_use_sd_models") != 0.f)
		return sdModelIndex;

	return hdModelIndex;
}

void SET_MODEL(edict_t *e, const char *m)
{
	if (CVAR_GET_FLOAT("vr_use_sd_models") != 0.f)
	{
		char* sdModel = GetSDModel(m);
		if (sdModel != nullptr)
		{
			SET_MODEL2(e, sdModel);
			return;
		}
	}

	SET_MODEL2(e, m);
}

int MODEL_INDEX(const char *m)
{
	if (CVAR_GET_FLOAT("vr_use_sd_models") != 0.f)
	{
		char* sdModel = GetSDModel(m);
		if (sdModel != nullptr)
		{
			return MODEL_INDEX2(sdModel);
		}
	}

	return MODEL_INDEX2(m);
}

bool gSDModelsEnabled{ false };
void UTIL_UpdateSDModels()
{
	bool sdModelsEnabled = CVAR_GET_FLOAT("vr_use_sd_models") != 0.f;
	if (sdModelsEnabled != gSDModelsEnabled)
	{
		gSDModelsEnabled = sdModelsEnabled;
		CBaseEntity* pEntity = nullptr;
		while (UTIL_FindAllEntities(&pEntity))
		{
			if (pEntity->pev->model)
			{
				const char* modelName = STRING(pEntity->pev->model);
				if (gSDModelsEnabled)
				{
					char* sdModel = GetSDModel(modelName);
					if (sdModel != nullptr)
					{
						SET_MODEL2(pEntity->edict(), sdModel);
					}
				}
				else if (!gSDModelsEnabled && std::string{ modelName }.find("models/SD/") != 0)
				{
					SET_MODEL2(pEntity->edict(), GetHDModel(modelName));
				}
			}
		}
	}
}
