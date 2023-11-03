using System.Text.Json.Nodes;

namespace ImageSearcher;

public partial class MainPage : ContentPage
{

	private readonly string PEXELS_API_KEY = "Iefgoke2pNoeVIYbC4cJUmnKVn4uvywYj1nx3NlCSKoMCBI0HStAXApc";
	private static HttpClient client = new()
	{
		BaseAddress = new Uri("https://api.pexels.com/v1/"),
	};

	public MainPage()
	{
		client.DefaultRequestHeaders.Add("Authorization", PEXELS_API_KEY);
		InitializeComponent();
	}

	async void OnSearchClick(object sender, EventArgs args)
	{
		if (imageSearch.Text == "" || imageSearch.Text == null)
		{
			return;
		}
		await GetImage();
		Console.WriteLine("Clicked");
	}

	public async Task GetImage()
	{
		string query = imageSearch.Text;
		using HttpResponseMessage response = await client.GetAsync("search?query=" + query + "&per_page=1");

		var jsonResponse = await response.Content.ReadAsStringAsync();

		int urlInex = jsonResponse.IndexOf("original"); // Workaround to avoid using json
		if (urlInex == -1)
		{
			return;
		}
		int firstIndex = urlInex + 11;
		char curr = jsonResponse[firstIndex];	
		int i = firstIndex;
		while (curr != '"')
		{
			i++;
			curr = jsonResponse[i];
		}
		string newUrl = jsonResponse.Substring(firstIndex, i-firstIndex);
		Response.Text = response.StatusCode.ToString() + "\n" + jsonResponse + "\n" +
		 urlInex + " : " + jsonResponse[urlInex] + "\n" + firstIndex + " : " + jsonResponse[firstIndex]
		 + "\n" + i + " : " + jsonResponse[i] + "\n" + newUrl;	// Debugging

		 Result.Source = newUrl;
	}
}