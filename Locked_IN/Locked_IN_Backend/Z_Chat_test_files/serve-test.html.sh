#!/bin/bash
# Simple HTTP server script to serve TestSignalR.html
# This allows the HTML file to be accessed via http:// instead of file://

PORT=${1:-8080}
FILE=${2:-TestSignalR.html}

echo "Starting HTTP server on port $PORT..."
echo "Open http://localhost:$PORT/$FILE in your browser"
echo "Press Ctrl+C to stop"

# Check if Python 3 is available
if command -v python3 &> /dev/null; then
    python3 -m http.server $PORT
# Check if Python 2 is available
elif command -v python &> /dev/null; then
    python -m SimpleHTTPServer $PORT
# Check if Node.js is available
elif command -v node &> /dev/null; then
    npx http-server -p $PORT
else
    echo "Error: No HTTP server found. Please install Python 3, Python 2, or Node.js"
    exit 1
fi

