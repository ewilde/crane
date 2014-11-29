param(
    [Parameter(Position=0,Mandatory=0)]
    [string]$version = "0.0.1.0",
    [Parameter(Position=1,Mandatory=0)]
    [string]$configuration = "Debug"
)

  $build_dir = (Split-Path $psake.build_script_file)
  $build_artifacts_dir = "$build_dir\..\build-output\"
  $src_dir = "$build_dir\..\src"
  $sln_filename = "%context.Configuration.ProjectName%.sln"
  $sln_filepath = "$src_dir\$sln_filename" 
 

Import-Module (Join-Path $build_dir 'psake-ext.psm1')
FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends BuildCrane

Task BuildCrane -Depends Info, Clean, Build

Task Info {
  Write-Host build_dir: $build_dir
  Write-Host build_artifacts_dir: $build_artifacts_dir
  Write-Host src_dir: $src_dir
  Write-Host sln_filename: $sln_filename
  Write-Host sln_filepath: $sln_filepath
}

Task Build -Depends Clean, NugetRestore { 
    Write-Host "Building $sln_filename ($configuration)" -ForegroundColor Green
    Exec { msbuild "$sln_filepath" /t:Build /p:Configuration=$configuration /v:quiet /p:OutDir=$build_artifacts_dir } 
}

Task Clean {
    Write-Host "Creating build-output directory" -ForegroundColor Green
    if (Test-Path $build_artifacts_dir) 
    {   
        rd $build_artifacts_dir -rec -force | out-null
    }
    
    mkdir $build_artifacts_dir | out-null
    
    Write-Host "Cleaning $sln_filename ($configuration)" -ForegroundColor Green
    Exec { msbuild $sln_filepath /t:Clean /p:Configuration=$configuration /v:quiet } 
}

Task NugetRestore -Depends NugetExists { 
   & $build_dir\nuget.exe @('restore', $sln_filepath)
}

Task NugetExists { 
    Invoke-DownloadNuget $build_dir #doesn't download if exists
}