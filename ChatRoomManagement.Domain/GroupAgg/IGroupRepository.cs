﻿using _01_framework.Domain;
using ChatRoomManagement.Application.Contracts.Group;

namespace ChatRoomManagement.Domain.GroupAgg
{
	public interface IGroupRepository : IRepository<long, Group>
	{
		Task<List<GroupViewModel>> GetGroups();
		Task<EditGroup> GetDetails(long groupId);
	}


}