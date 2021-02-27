#include <string>
#include <filesystem>
#include <regex>
#include <memory>
#include <unordered_map>

#include "extdll.h"
#include "util.h"
#include "cbase.h"

//File originally used to load SD variants of models. TODO: Remove this file entirely.

inline std::filesystem::path GetPathFor(const std::string& file)
{
	std::filesystem::path relativePath = UTIL_GetGameDir() + file;
	return std::filesystem::absolute(relativePath);
}

namespace
{
	std::unordered_map<std::string, std::shared_ptr<char>> gModelNames;
}

char* GetSafeModelCharPtr(const std::string& model)
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

extern const char* GetAnimatedWeaponModelName(const char* modelName);

int PRECACHE_MODEL2(const char* s)
{
	const char* anims = GetAnimatedWeaponModelName(s);
	if (anims != s)
		PRECACHE_MODEL3(const_cast<char*>(anims));
	return PRECACHE_MODEL3(s);
}

int PRECACHE_GENERIC2(const char* s)
{
	const char* anims = GetAnimatedWeaponModelName(s);
	if (anims != s)
		PRECACHE_GENERIC3(const_cast<char*>(anims));
	return PRECACHE_GENERIC3(s);
}

int PRECACHE_MODEL(const char* s)
{
	int hdModelIndex = PRECACHE_MODEL2(s);

	return hdModelIndex;
}

int PRECACHE_GENERIC(const char* s)
{
	int hdModelIndex = PRECACHE_GENERIC2(s);

	return hdModelIndex;
}

void SET_MODEL(edict_t* e, const char* m)
{
	SET_MODEL2(e, m);
}

int MODEL_INDEX(const char* m)
{
	return MODEL_INDEX2(m);
}

void UTIL_SetModelKeepSize(edict_t* pent, const char* model)
{
	Vector mins = pent->v.mins;
	Vector maxs = pent->v.maxs;
	SET_MODEL2(pent, model);
	UTIL_SetSize(&pent->v, mins, maxs);
}