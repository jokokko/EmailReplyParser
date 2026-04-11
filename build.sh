#!/usr/bin/env bash
set -euo pipefail

dotnet tool restore
dotnet paket restore

dotnet fake run build.fsx "$@"
