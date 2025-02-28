class Program
{
    static async Task Main(string[] args)
    {

        DotnetAPICheck dotnetAPICheck = new DotnetAPICheck("DotNet_React");
        await dotnetAPICheck.DotnetAPIVerification();
    
    }
}