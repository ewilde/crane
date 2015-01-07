param(
  [string]$build_dir
)

Import-Module (Join-Path $build_dir "psake-ext.psm1") -Force
Import-Module (Join-Path $build_dir "modules/types.psm1") -Force
Import-Module (Join-Path $build_dir "modules/generaltasks.psm1") -Force
