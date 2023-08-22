﻿
namespace judo_univ_rennes.Contracts
{
	public interface IChatRoomService
	{
        Task<Guid> CreateRoom(string connectionId);

        Task<Guid> GetRoomForConnectionId(string connectionId);
    }
}

