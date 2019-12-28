export enum JobRequestHTTPMethod {
    GET   = 'GET',
    HEAD  = 'HEAD',
}

export type JobRequestFormFields = {
    links     : string;
    method    : JobRequestHTTPMethod;
    redirects : number;
    threads   : number;
    timeout   : number;
    userAgent : string;
}