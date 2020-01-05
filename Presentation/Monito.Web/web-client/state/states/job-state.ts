import { JobModel } from "~/types/job";

export type JobState = {
	job: JobModel | null,
	log: {
		messages: string[];
	},
	polling: {
		status: boolean;
	}
}

export const getInitialJobState = (): JobState => ({
	job: null,
	log: {
		messages: []
	},
	polling: {
		status: false
	}
});