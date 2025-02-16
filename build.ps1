dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "`nStarting MUD...`n" -ForegroundColor Green
    dotnet run --project src/MudEngine.Console
} 