#!/bin/bash

# Exit on error
set -e

echo "Starting smart .NET wrapper for Node.js runtime..."

# Install .NET 8.0 SDK
echo "Installing .NET 8.0..."
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --install-dir $HOME/.dotnet

# Add .NET to PATH
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$HOME/.dotnet

echo ".NET installation complete. Checking version..."
dotnet --version

# Publish the project
echo "Publishing project..."
dotnet publish spms/spms.csproj -c Release -o out

# Clear the PORT if it's set by Render (Render sets PORT but .NET prefers ASPNETCORE_URLS or --urls)
# Or just use the PORT env var
echo "Starting application on port $PORT..."
dotnet out/spms.dll --urls "http://0.0.0.0:${PORT:-10000}"
