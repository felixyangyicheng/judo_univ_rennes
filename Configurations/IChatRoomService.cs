
using System;
namespace judo_univ_rennes.Configurations
{
	public interface IChatRoomService
	{
        Task<Guid> CreateRoom(string connectionId);

        Task<Guid> GetRoomForConnectionId(string connectionId);
    }
}

