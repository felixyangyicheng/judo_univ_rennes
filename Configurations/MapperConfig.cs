
namespace judo_univ_rennes.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {


            CreateMap<ApiUser, UserRegisterDto>().ReverseMap();
            CreateMap<Command, CommandDto>()
                .ForMember(a => a.ApiUserName, d => d.MapFrom(map => $"{map.ApiUser.UserName}"))
                .ForMember(a => a.ApiUserId, d => d.MapFrom(map => $"{map.ApiUser.Id}"))
                .ReverseMap();
            CreateMap<Comment, CommentDto>()
                .ForMember(a => a.ApiUserName, d => d.MapFrom(map => $"{map.ApiUser.UserName}"))
                .ForMember(a => a.ApiUserId, d => d.MapFrom(map => $"{map.ApiUser.Id}"))
                .ForMember(a => a.PostId, d => d.MapFrom(map => $"{map.Post.Id}"))
                .ReverseMap();
            CreateMap<Post, PostDto>()
                .ForMember(a => a.ApiUserName, d => d.MapFrom(map => $"{map.ApiUser.UserName}"))
                .ForMember(a => a.ApiUserId, d => d.MapFrom(map => $"{map.ApiUser.Id}"))
                .ForMember(a => a.Comments, d => d.MapFrom(x => x.Comments.ToList()))
                .ReverseMap();
            CreateMap<PostDto, PostViewModel>()
                .ForMember(a => a.ApiUserName, d => d.MapFrom(map => $"{map.ApiUserName}"))
  
                .ForMember(a => a.Comments, d => d.MapFrom(x => x.Comments.ToList()))
                .ReverseMap();
            CreateMap<Post, PostViewModel>()
                .ForMember(a => a.ApiUserName, d => d.MapFrom(map => $"{map.ApiUser.UserName}"))
                .ForMember(a => a.DisplayContent, d => d.MapFrom(map => $"{(MarkupString)map.Content}"))
                .ForMember(a => a.Comments, d => d.MapFrom(x => x.Comments.ToList()))
                .ReverseMap();
            CreateMap<IndexMarkdown, IndexMarkdownDto>()
                .ReverseMap();
            CreateMap<News, NewsDto>()
                .ReverseMap();
        }
    }
}
