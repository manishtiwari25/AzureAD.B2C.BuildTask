"Copying *.dlls from bin\Debug"
get-childitem -path "..\bin\Debug" -filter *.dll -recurse | copy-item -destination "AzureADB2CBuildTask" 
"Copied"
"Creating Extension"
$path = npm prefix -g
$tfxCli = Join-Path $path "node_modules\tfx-cli\_build\tfx-cli.js"
Invoke-Expression -Command "node $tfxCli extension create --manifest-globs vss-extension.json"
"Created"