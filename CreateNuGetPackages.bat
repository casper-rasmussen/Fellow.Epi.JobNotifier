SETLOCAL

set "module=Fellow.Epi.JobNotifier"
set output="../ModulePackages/%module%"

rmdir /s /q %output%
mkdir %output%

nuget pack "Fellow.Epi.JobNotifier\Fellow.Epi.JobNotifier.csproj" -Build -OutputDirectory %output% -Prop Configuration=Release;Platform=AnyCPU