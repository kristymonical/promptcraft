$SERVICE_NAME = "MGPToolkit"
$DISPLAY_NAME = "MacGreggor Partners Toolkit"
$DESCRIPTION = "Toolkit for Lonza"
$APP_PATH = "C:\MGPToolkit"
$EXE_PATH = $APP_PATH + "\softbot-cloud-platform.exe"

If(Get-WmiObject -Class Win32_Service -Filter 'Name="MGPToolkit"') {
    Start-Service $SERVICE_NAME
    "Service `"" + $SERVICE_NAME + "`" exists and has been started."
    exit
}

$currentUser = (Get-WmiObject -Class Win32_Process -Filter 'Name="explorer.exe"').GetOwner()
$CREDENTIAL = $currentUser.Domain + "\" + $currentUser.User

$acl = Get-Acl $APP_PATH
$aclRuleArgs = $CREDENTIAL, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl $EXE_PATH

"Creating service..."
New-Service -Name $SERVICE_NAME -BinaryPathName $EXE_PATH -Credential $CREDENTIAL -Description $DESCRIPTION -DisplayName $DISPLAY_NAME -StartupType Automatic
"Service `"" + $SERVICE_NAME + "`" created."

Start-Service $SERVICE_NAME
"Service `"" + $SERVICE_NAME + "`" started."