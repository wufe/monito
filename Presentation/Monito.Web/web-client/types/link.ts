export type LinkModel = {
	id                 : number;
	url                : string;
	output             : string;
	statusCode         : number;
	uuid               : string;
	createdAt          : string;
	updatedAt          : string;
	redirectsFromLinkId: number | null;
	redirectsToLinkId  : number | null;
	requestID          : number;
}