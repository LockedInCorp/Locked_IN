# PowerShell script to serve TestSignalR.html
# Usage: .\serve-test.html.ps1 [port]
# Default port: 8080

param(
    [int]$Port = 8080
)

$file = "TestSignalR.html"

Write-Host "Starting HTTP server on port $Port..." -ForegroundColor Green
Write-Host "Open http://localhost:$Port/$file in your browser" -ForegroundColor Yellow
Write-Host "Press Ctrl+C to stop" -ForegroundColor Gray

# Use Python if available
if (Get-Command python -ErrorAction SilentlyContinue) {
    python -m http.server $Port
}
# Use Node.js if available
elseif (Get-Command node -ErrorAction SilentlyContinue) {
    npx http-server -p $Port
}
else {
    Write-Host "Error: No HTTP server found. Please install Python or Node.js" -ForegroundColor Red
    exit 1
}

