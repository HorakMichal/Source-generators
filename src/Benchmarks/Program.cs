using BenchmarkDotNet.Running;

//dotnet run -c Release -- --filter *Regex*
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);