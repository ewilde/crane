
function Add-Includes {
  param(
    [string]$module_dir
  )
  Import-Module (Join-Path $module_dir 'psake-ext.psm1') -Force
  Import-Module (Join-Path $module_dir '/modules/types.psm1') -Force
  Import-Module (Join-Path $module_dir '/modules/generaltasks.psm1') -Force
}

export-modulemember -function Add-Includes
