using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }


    public static int getTotalScoredGoals(string team, int year)
    {
        int page = 1;
        bool hasMorePages = true;

        int totalTeam1Goals = 0;

        while (hasMorePages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    MatchResponse matchResponse = JsonConvert.DeserializeObject<MatchResponse>(responseBody);

                    foreach (var matchResult in matchResponse.Data)
                    {
                        totalTeam1Goals += matchResult.Team1Goals;
                    }

                    hasMorePages = page < matchResponse.TotalPages;

                    page++;
                }
                else
                {
                    Console.WriteLine($"Erro ao fazer a solicitação: {response.StatusCode}");
                    hasMorePages = false;
                }
            }
        }

        int pageTeam2 = 1;
        bool hasMorePagesTeam2 = true;

        int totalTeam2Goals = 0;

        while (hasMorePagesTeam2)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={pageTeam2}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    MatchResponse matchResponse = JsonConvert.DeserializeObject<MatchResponse>(responseBody);

                    foreach (var matchResult in matchResponse.Data)
                    {
                        totalTeam2Goals += matchResult.Team2Goals;
                    }

                    hasMorePagesTeam2 = pageTeam2 < matchResponse.TotalPages;

                    pageTeam2++;
                }
                else
                {
                    Console.WriteLine($"Erro ao fazer a solicitação: {response.StatusCode}");
                    hasMorePagesTeam2 = false;
                }
            }
        }

        return totalTeam1Goals + totalTeam2Goals;
    }

    public class MatchResponse
    {
        public int Page { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
        public int Total { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        public List<MatchResult> Data { get; set; }
    }

    public class MatchResult
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public string Round { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
    }
}