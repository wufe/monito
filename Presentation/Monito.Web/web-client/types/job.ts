import { LinkModel } from "~/types/link";

export enum JobHTTPMethod {
    GET  = "GET",
    HEAD = "HEAD",
}

export type JobRequestFormFields = {
    links     : string;
    method    : JobHTTPMethod;
    redirects : number;
    threads   : number;
    timeout   : number;
    userAgent : string;
}

export enum JobRequestType {
    SIMPLE = "Simple",
    BATCH  = "Batch",
}

export enum JobStatus {
    INCOMPLETE   = "Incomplete",
    READY        = "Ready",
    ACKNOWLEDGED = "Acknowledged",
    INPROGRESS   = "InProgress",
    DONE         = "Done",
}

// #region Responses
export type SaveJobResponse = {
    userUUID: string;
    requestUUID: string;
}

export type JobStatuModel = {
    status   : JobStatus;
    updatedAt: string;
}

export type JobModel = JobStatuModel & {
    id       : number;
    type     : JobRequestType;
    createdAt: string;
    links    : LinkModel[];
    options  : {
        method   : JobHTTPMethod;
        redirects: number;
        threads  : number;
        timeout  : number;
        userAgent: string;
    }
}
// #endregion