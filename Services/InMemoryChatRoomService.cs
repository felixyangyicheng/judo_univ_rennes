﻿using System;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;

namespace judo_univ_rennes.Services
{
	public class InMemoryChatRoomService: IChatRoomService
    {
        private readonly Dictionary<Guid, ChatRoom> _roomInfo
    = new Dictionary<Guid, ChatRoom>();

        public Task<Guid> CreateRoom(string connectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoom
            {
                OwnerConnectionId = connectionId
            };

            return Task.FromResult(id);
        }

        public Task<Guid> GetRoomForConnectionId(string connectionId)
        {
            var foundRoom = _roomInfo.FirstOrDefault(
                x => x.Value.OwnerConnectionId == connectionId);

            if (foundRoom.Key == Guid.Empty)
                throw new ArgumentException("Invalid connection ID");

            return Task.FromResult(foundRoom.Key);
        }
    }
}

