namespace ArkFury.Core.RCON.ARK
{
    public static class RCONCommands
    {
        /// <summary>
        /// {0} = SteamId
        /// {1} = BlueprintPath
        /// {2} = StackSize
        /// {3} = Quality
        /// {4} = GiveBlueprint
        /// </summary>
        public static string GIVE_ITEM => @"giveitem {0} {1} {2} {3} 0";

        /// <summary>
        /// {0} = SteamId
        /// {1} = Blueprint Path
        /// {2} = Level
        /// </summary>
        public static string GIVE_DINO => @"spawndino {0} {1} {2} 1 0 0 0";

        /// <summary>
        /// {0} = SteamId
        /// {1} = How Much
        /// {2} = From Tribe Share
        /// {3} = Prevent Sharing With Tribe
        /// </summary>
        public static string ADD_EXPERIENCE_TO_PLAYER => @"addexperience {0} {1} {2} {3}";

        /// <summary>
        /// {0} = SteamId
        /// {1} = X
        /// {2} = Y
        /// {3} = Z
        /// </summary>
        public static string TELEPORT_PLAYER_TO_COORDINATE => @"setplayerpos {0} {1} {2} {3}";

        /// <summary>
        /// {0} = SteamId
        /// </summary>
        public static string GET_PLAYER_POSITION => @"getplayerpos {0}";

        /// <summary>
        /// {0} = Moving Player
        /// {1} = Destination Player
        /// </summary>
        public static string TELEPORT_PLAYER_TO_PLAYER => @"teleporttoplayer {0} {1}";

        /// <summary>
        /// {0} = SteamId
        /// </summary>
        public static string LIST_PLAYER_DINOS => @"listplayerdinos {0}";

        /// <summary>
        /// {0} = SteamId
        /// </summary>
        public static string GET_PLAYER_TRIBE => @"gettribeidofplayer {0}";

        /// <summary>
        /// {0} = DinosaurId
        /// </summary>
        public static string GET_DINOSAUR_POSITION => "getdinopos {0}";

        /// <summary>
        /// {0} = DinosaurId1
        /// {1} = DinosaurId2
        /// {1} = X
        /// {2} = Y
        /// {3} = Z
        /// </summary>
        public static string TELEPORT_DINOSAUR_TO_COORDINATE => "setdinopos {0} {1} {2} {3} {4}";

        /// <summary>
        /// {0} = DinosaurId
        /// {1} = How Much
        /// </summary>
        public static string ADD_EXPERIENCE_TO_DINOSAUR => @"adddinoexperience {0} {1}";

        /// <summary>
        /// {0} = DinosaurId
        /// {1} = How Much
        /// </summary>
        public static string KILL_DINOSAUR => @"killdino {0}";

        /// <summary>
        /// {0} = SteamId
        /// {1} = Blueprint Path
        /// </summary>
        public static string UNLOCK_ENGRAM => "unlockengram {0} {1}";

        /// <summary>
        /// {0} = DinosaurId
        /// {1} = Percentage
        /// </summary>
        public static string SET_IMPRINT_PERCENTAGE => "setimprintquality {0} {1}";

        /// <summary>
        /// {0} = DinosaurId
        /// {1} = SteamId
        /// </summary>
        public static string SET_IMPRINT_TO_PLAYER => "setimprinttoplayer {0} {1}";

        /// <summary>
        /// {0} = SteamId
        /// </summary>
        public static string GET_TRIBE_ID_PLAYER_LIST => "GetTribeIdPlayerList {0}";
    }
}
