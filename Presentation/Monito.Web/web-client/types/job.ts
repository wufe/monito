export enum JobRequestHTTPMethod {
    GET  = "GET",
    HEAD = "HEAD",
}

export type JobRequestFormFields = {
    links     : string;
    method    : JobRequestHTTPMethod;
    redirects : number;
    threads   : number;
    timeout   : number;
    userAgent : string;
}

export enum JobRequestType {
    SIMPLE = "Simple",
    BATCH  = "Batch",
}

export enum JobRequestStatus {
    INCOMPLETE = "Incomplete",
    READY      = "Ready",
    INPROGRESS = "InProgress",
    DONE       = "Done",
}