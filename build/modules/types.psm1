$contextclass = new-object psobject -Property @{
  root_dir = $null
  build_dir = $null
  sln_file_info = $null
  build_version = $null
  build_artifacts_dir = $null
  configuration = $null
  project_name = $null
}

function ContextClass {
  param(
    [Parameter(Mandatory=$true)]
    [String]$psake_build_script_dir,
    [Parameter(Mandatory=$true)]
    [String]$relative_solution_path,
    [String]$configuration,
    [String]$build_number
  )

  $context = $ContextClass.psobject.copy()
  $context.root_dir = Resolve-Path "$psake_build_script_dir\.."
  $context.build_dir = $psake_build_script_dir
  $context.sln_file_info = Get-Item -Path (Resolve-Path (Join-Path $psake_build_script_dir $relative_solution_path))
  $context.build_version = "$(Get-Content -Path "$($context.root_dir)\VERSION.txt").$build_number"
  $context.build_artifacts_dir = "$($context.root_dir)\build-output"
  $context.configuration = $configuration

  $rootdir = Get-Item $context.root_dir
  $context.project_name = $rootdir.Parent.Name

  $context
}

export-modulemember -function ContextClass
