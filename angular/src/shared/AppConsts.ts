export class AppConsts {

    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'InventoryManagementSystem'
    };

    static readonly authorization = {
        encryptedAuthTokenName: 'enc_auth_token'
    };
    //GraphData
    static readonly graphData = {
        ThisWeek: 'ThisWeek',
        ThisMonth: 'ThisMonth',
        ThisYear: 'ThisYear',
        AllYear: 'AllYear'
    };
}
