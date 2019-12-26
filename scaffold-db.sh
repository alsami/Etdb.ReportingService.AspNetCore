#!/usr/bin/env bash
export scaffold_path=src/Etdb.ReportingService.Scaffolder/Etdb.ReportingService.Scaffolder.csproj
export dll_path=src/Etdb.ReportingService.Scaffolder/bin/Debug/netcoreapp3.1/Etdb.ReportingService.Scaffolder.dll
echo running context scaffold
dotnet build ${scaffold_path}
dotnet ${dll_path}
