using Edu.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using Edu.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Edu.BlazorWebAssembly.Client.Pages.Education.Autocomplete;

public class PostAutocomplete : MudAutocomplete<int>
{
    [Inject]
    private IStringLocalizer<PostAutocomplete> L { get; set; } = default!;
    [Inject]
    private IPostsClient PostsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<PostDto> _posts = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Post"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchPosts;
        ToStringFunc = GetPostName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one post to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => PostsClient.GetAsync(_value), Snackbar) is { } post)
        {
            _posts.Add(post.Adapt<PostDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<int>> SearchPosts(string value)
    {
        var filter = new SearchPostsRequest
        {
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => PostsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfPostDto response)
        {
            _posts = response.Data.OrderBy(x => x.Name).ToList();
        }

        return _posts.Select(x => x.Id);
    }

    private string GetPostName(int id)
    {
        var result = _posts.Find(b => b.Id == id);
        if (result is null)
            return string.Empty;
        return $"{result.Name}";
    }
}