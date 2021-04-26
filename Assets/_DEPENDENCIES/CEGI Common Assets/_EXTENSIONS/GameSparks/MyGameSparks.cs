#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
	}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_LISTEN : GSTypedRequest<LeaderboardDataRequest_LISTEN,LeaderboardDataResponse_LISTEN>
	{
		public LeaderboardDataRequest_LISTEN() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "LISTEN");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_LISTEN (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_LISTEN SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_LISTEN : GSTypedRequest<AroundMeLeaderboardRequest_LISTEN,AroundMeLeaderboardResponse_LISTEN>
	{
		public AroundMeLeaderboardRequest_LISTEN() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "LISTEN");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_LISTEN (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_LISTEN SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_LISTEN : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_LISTEN(GSData data) : base(data){}
	}
	
	public class LeaderboardDataResponse_LISTEN : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_LISTEN(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> Data_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> First_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> Last_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_LISTEN : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_LISTEN(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> Data_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> First_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LISTEN> Last_LISTEN{
			get{return new GSEnumerable<_LeaderboardEntry_LISTEN>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_LISTEN(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {


}
