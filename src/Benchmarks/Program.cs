using BenchmarkDotNet.Running;

//dotnet run -c Release -- --filter *Regex*
//dotnet run -c Release -- --filter *Logger*
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);