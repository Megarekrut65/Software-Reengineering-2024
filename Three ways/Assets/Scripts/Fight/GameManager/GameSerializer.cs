using System;

namespace Fight.GameManager
{
    public static class GameSerializer
    {
        public static object DeserializeGameEvent(byte[] data)
        {
            GameEvent result = new GameEvent
            {
                isSelected = BitConverter.ToBoolean(data, 0),
                attackIndex = BitConverter.ToInt32(data, 1),
                protectIndex = BitConverter.ToInt32(data, 5),
                hp = BitConverter.ToInt32(data, 9),
                isAttackChance = BitConverter.ToBoolean(data, 13),
                isProtectChance = BitConverter.ToBoolean(data, 14)
            };
            return result;
        }
        public static byte[] SerializeGameEvent(object obj)
        {
            GameEvent gameEvent = (GameEvent)obj;
            byte[] result = new byte[ 1 + 4 + 4 + 4 + 1 + 1];
            BitConverter.GetBytes(gameEvent.isSelected).CopyTo(result, 0);
            BitConverter.GetBytes(gameEvent.attackIndex).CopyTo(result, 1);
            BitConverter.GetBytes(gameEvent.protectIndex).CopyTo(result, 5);
            BitConverter.GetBytes(gameEvent.hp).CopyTo(result, 9);
            BitConverter.GetBytes(gameEvent.isAttackChance).CopyTo(result, 13);
            BitConverter.GetBytes(gameEvent.isProtectChance).CopyTo(result, 14);

            return result;
        }
        public static object DeserializeGameInfo(byte[] data)
        {
            GameInfo result = new GameInfo
            {
                isReady = BitConverter.ToBoolean(data, 0),
                indexOfAvatar = BitConverter.ToInt32(data, 1),
                points = BitConverter.ToInt32(data, 5),
                code = BitConverter.ToInt32(data, 9),
                maxHP = BitConverter.ToInt32(data, 13),
                isHost = BitConverter.ToBoolean(data, 17)
            };

            return result;
        }
        public static byte[] SerializeGameInfo(object obj)
        {
            GameInfo gameInfo = (GameInfo)obj;
            byte[] result = new byte[ 1 + 4 + 4 + 4 + 4 + 1];
            BitConverter.GetBytes(gameInfo.isReady).CopyTo(result, 0);
            BitConverter.GetBytes(gameInfo.indexOfAvatar).CopyTo(result, 1);
            BitConverter.GetBytes(gameInfo.points).CopyTo(result, 5);
            BitConverter.GetBytes(gameInfo.code).CopyTo(result, 9);
            BitConverter.GetBytes(gameInfo.maxHP).CopyTo(result, 13);
            BitConverter.GetBytes(gameInfo.isHost).CopyTo(result, 17);

            return result;
        }
    }
}