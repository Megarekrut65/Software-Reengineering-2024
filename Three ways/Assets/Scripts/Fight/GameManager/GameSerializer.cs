using System;
using Fight.EventHandler;

namespace Fight.GameManager
{
    public static class GameSerializer
    {
        public static object DeserializeGameEvent(byte[] data)
        {
            GameEvent result = new GameEvent
            {
                IsSelected = BitConverter.ToBoolean(data, 0),
                AttackIndex = (Direction)BitConverter.ToInt32(data, 1),
                ProtectIndex = (Direction)BitConverter.ToInt32(data, 5),
                Hp = BitConverter.ToInt32(data, 9),
                IsAttackChance = BitConverter.ToBoolean(data, 13),
                IsProtectChance = BitConverter.ToBoolean(data, 14)
            };
            return result;
        }
        public static byte[] SerializeGameEvent(object obj)
        {
            GameEvent gameEvent = (GameEvent)obj;
            byte[] result = new byte[ 1 + 4 + 4 + 4 + 1 + 1];
            BitConverter.GetBytes(gameEvent.IsSelected).CopyTo(result, 0);
            BitConverter.GetBytes((int)gameEvent.AttackIndex).CopyTo(result, 1);
            BitConverter.GetBytes((int)gameEvent.ProtectIndex).CopyTo(result, 5);
            BitConverter.GetBytes(gameEvent.Hp).CopyTo(result, 9);
            BitConverter.GetBytes(gameEvent.IsAttackChance).CopyTo(result, 13);
            BitConverter.GetBytes(gameEvent.IsProtectChance).CopyTo(result, 14);

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