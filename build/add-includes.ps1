param(
  [string]$build_dir
)

Import-Module (Join-Path $build_dir "modules\buildtasks.psm1") -Force
Import-Module (Join-Path $build_dir "modules\contextclass.psm1") -Force
Import-Module (Join-Path $build_dir "modules\chocolateytasks.psm1") -Force
Import-Module (Join-Path $build_dir "modules\generatedocs.psm1") -Force
Import-Module (Join-Path $build_dir "modules\nugettasks.psm1") -Force
