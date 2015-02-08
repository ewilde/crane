$contextclass = new-object psobject -Property @{
  root_dir = $null
  build_dir = $null
  sln_file_info = $null
  build_version = $null
  build_artifacts_dir = $null
  packages_folder = $null
  configuration = $null
  chocolatey_api_key = $null
  chocolatey_api_url = $null
  teamcity_build = $null
  is_git_repo = $null
  solution_context = $null
}

function ContextClass {
  param(
    [Parameter(Mandatory=$true)]
    [String]$psake_build_script_dir,
    [Parameter(Mandatory=$true)]
    [String]$relative_solution_path,
    $props
  )

  $context = $ContextClass.psobject.copy()
  $context.root_dir = Resolve-Path "$psake_build_script_dir\.."
  $context.build_dir = $psake_build_script_dir
  $context.sln_file_info = Get-Item -Path (Resolve-Path (Join-Path $psake_build_script_dir $relative_solution_path))
  $context.build_version = "$(Get-Content -Path "$($context.root_dir)\VERSION.txt").$($props.build_number)"
  $context.build_artifacts_dir = "$($context.root_dir)\build-output"
  $context.is_git_repo = Test-Path (Join-Path $context.root_dir '.git')
  $context.packages_folder = Join-Path $context.sln_file_info.Directory.FullName "packages"

  $context.configuration = $props.configuration
  $context.chocolatey_api_key = $props.chocolateyApiKey
  $context.chocolatey_api_url = $props.chocolateyApiUrl
  $context.teamcity_build = $props.teamcityBuild

  Import-Module "$($context.build_dir)\builtmodules\Crane.PowerShell.dll"
  $context.solution_context = Get-CraneSolutionContext -Path $($context.sln_file_info)

  $context
}

Export-ModuleMember -function ContextClass
