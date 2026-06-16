#Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope CurrentUser
if ($PSVersionTable.PSVersion.Major -lt 7){
    Write-Host "Powershell incompatible version.`r`n Your current version is $($PSVersionTable.PSVersion)`r`n"
    return
}
$ExeName = 'mYPMS.exe'
$ServerCertFileName = ""
$ServerCertPassword = ""
function Show-Menu {
    param (
        [string]$Title = 'megafa Yakand PMS installation'
    )
    Clear-Host
    
    Write-Host "================ $Title ================"
    
    Write-Host "0: Press '0' to check the http,https port availability"
    Write-Host "1: Press '1' to install root certificate"
    Write-Host "2: Press '2' to create a child certificate"
    Write-Host "3: Press '3' to update appsetting.json"
    Write-Host "4: Press '4' to install the mYPMS service."
    Write-Host "5: Press '5' to start the mYPMS service."
    Write-Host "6: Press '6' to stop the mYPMS service"
    Write-Host "7: Press '7' to remove the mYPMS service"
    Write-Host "8: Press '8' to check the mYPMS service status"
    Write-Host "Q: Press 'Q' to quit."
}
function Show-Http-Port {
    get-nettcpconnection | Where-Object localport -eq 80  | Select-Object local*, remote*, state, @{Name = "Process"; Expression = { (Get-Process -Id $_.OwningProcess).ProcessName } }
    get-nettcpconnection | Where-Object localport -eq 443  | Select-Object local*, remote*, state, @{Name = "Process"; Expression = { (Get-Process -Id $_.OwningProcess).ProcessName } }
}
function Install-PMS {
    Write-Host "Installing the service. Please wait..."
    if (Test-Path $ExeName) {
        New-Service -Name mYPMS -BinaryPathName "$(Join-Path $(Get-Location) $ExeName) --contentRoot $(Get-Location)" -Credential "LocalSystem" -Description "megafa Yakand Parking Management System" -DisplayName "Parking Management System" -StartupType Automatic    
    }
    else {
        Write-Host "Process $ExeName not found. please check the current folder"
    }
}
function Start-PMS {
    Write-Host "Starting the service. Please wait..."
    Start-Service -Name mYPMS
}
function Stop-PMS {
    Write-Host "Stopping the service. Please wait..."
    Stop-Service -Name mYPMS
}
function Uninstall-PMS {
    Write-Host "Removing the service. Please wait..."
    #$service = Get-WmiObject -Class Win32_Service -Filter "Name='mYPMS'"
    #$service.delete()
    Remove-Service -Name mYPMS
}
function Show-PMS {
    Write-Host "Getting the service status. Please wait..."
    Get-Service -Name mYPMS
}
function New-Cert {
    $hostname = Read-Host "Enter Root Certificate Common Name"
    $plainpass = Read-Host "Root Key Password"
    $password = ConvertTo-SecureString -String $plainpass -Force -AsPlainText
    $certroot = New-SelfSignedCertificate -DnsName $hostname -CertStoreLocation "Cert:\LocalMachine\My" -NotAfter (Get-Date).AddYears(20) -FriendlyName $hostname -KeyUsageProperty All -KeyUsage CertSign, CRLSign, DigitalSignature
    Get-ChildItem -Path Cert:\LocalMachine\My\$($certroot.Thumbprint) | Export-PfxCertificate -FilePath root_$($hostname).pfx -Password $password
    Export-Certificate -Cert Cert:\LocalMachine\My\$($certroot.Thumbprint) -FilePath root_$($hostname).crt
}
function Install-RootCert {
    $password = ConvertTo-SecureString -String 'megafaY@kand139' -Force -AsPlainText
    Import-Certificate -FilePath "root_mYPMS.yakand.com.crt" -CertStoreLocation 'Cert:\LocalMachine\Root'
    Import-PfxCertificate -FilePath "root_mYPMS.yakand.com.pfx" -CertStoreLocation 'Cert:\LocalMachine\My' -Password $password
}
function New-ChildCert {
    $rootcert = (Get-ChildItem -Path cert:\LocalMachine\Root\5554c37b1d8d1bd3bd274723752e09163a78f82b)
    if ($rootcert) {
        $child = Read-Host 'Enter Child Certificate Common Name'
        $plainpass = Read-Host "Child Key Password"
        $password = ConvertTo-SecureString -String $plainpass -Force -AsPlainText
        $signer = (Get-ChildItem -Path Cert:\LocalMachine\My\E43DDBA635BAC9D4C591AA17E1301D693F0ED0D9)
        $childcert = New-SelfSignedCertificate -DnsName $child -CertStoreLocation "Cert:\LocalMachine\My" -Signer $signer -NotAfter (Get-Date).AddYears(20) -FriendlyName $child
        Get-ChildItem -Path Cert:\LocalMachine\My\$($childcert.Thumbprint) | Export-PfxCertificate -FilePath child_$($child).pfx -Password $password
        Export-Certificate -Cert Cert:\LocalMachine\My\$($childcert.Thumbprint) -FilePath child_$($child).crt
        Import-Certificate -FilePath child_$($child).crt -CertStoreLocation 'Cert:\LocalMachine\Root'
        $plainpass | Out-File -FilePath child_$($child).txt
        $ServerCertFileName = "child_$($child).pfx"
        $ServerCertPassword = $plainpass
    }
    else {
        Write-Host "Root certificate not found. make sure you have installed root certificate in the Trusted Roor Certification Authorities"
    }
}
function Get-Thumbprint {
    $subject = Read-Host "Enter the Root Certificate Subject"
    Get-ChildItem  -Path Cert:\LocalMachine\Root | Where-Object { $_.Subject -Match $subject } | Select-Object FriendlyName, Thumbprint, Subject
}
function Update-Appsetting{
    $json = Get-Content 'appsettings.json' -Raw | ConvertFrom-Json
    $json.Kestrel.Endpoints.Https.Certificate.Path = $ServerCertFileName
    $json.Kestrel.Endpoints.Https.Certificate.Password = $ServerCertPassword
    $json | ConvertTo-Json -Depth 32 | Set-Content 'appsettings.json'
}
do {
    Show-Menu
    $selection = Read-Host "Please Make a Selection"
    switch ($selection) {
        '0' { Show-Http-Port }
        '1' { Install-RootCert } 
        '2' { New-ChildCert }
        '3' {}
        '4' { Install-PMS }
        '5' { Start-PMS }
        '6' { Stop-PMS }
        '7' { Uninstall-PMS }
        '8' { Show-PMS }
    }
    pause
}
until ($selection -eq 'q')