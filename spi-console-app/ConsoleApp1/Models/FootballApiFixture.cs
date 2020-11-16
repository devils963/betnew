namespace ConsoleApp1.types
{
    public class FootballApiFixture
    {
        public int id;

        public string homeTeam;

        public string awayTeam;

        public FootballApiFixture(int _id, string _homeTeam, string _awayTeam)
        {
            id = _id;
            homeTeam = _homeTeam;
            awayTeam = _awayTeam;
        }
    }
}
