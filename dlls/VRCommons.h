#pragma once

// Common defines and types for VR - Max Vollmer - 2019-04-07

#define VR_DUCK_START_HEIGHT 0
#define VR_DUCK_STOP_HEIGHT 20

#include <map>
#include "vector.h"

typedef int	string_t;
typedef int	BOOL;
class CBasePlayer;

enum class VRControllerID : int32_t {
	WEAPON = 0,
	HAND,
	INVALID
};

#include "VRStuff.h"

extern GlobalXenMounds gGlobalXenMounds;
extern VRLevelChangeData g_vrLevelChangeData;
