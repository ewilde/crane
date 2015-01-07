$contextclass = new-object psobject -Property @{
  build_dir = $null
  root_dir = $null
  build_artifacts_dir = $null
  sln_file_info = $null
  build_version = $null
}

function ContextClass {
  param(
    [Parameter(Mandatory=$true)]
    [String]$psake_build_script_dir,
    [Parameter(Mandatory=$true)]
    [String]$relative_solution_path,
    [String]$build_number
  )

  $context = $ContextClass.psobject.copy()
  $context.root_dir = Resolve-Path "$psake_build_script_dir\.."
  $context.build_dir = $psake_build_script_dir
  $context.sln_file_info = Get-Item -Path (Resolve-Path (Join-Path $psake_build_script_dir $relative_solution_path))
  $context.build_version = "$(Get-Content -Path "$($context.root_dir)\VERSION.txt").$build_number"
  $context.build_artifacts_dir = "$($context.root_dir)\build-output"

  $context
}

export-modulemember -function ContextClass
