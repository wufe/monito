import * as React from "react";
import { setLoadingActionBuilder } from "~/state/actions/application-actions";

export const APPLICATION_NAME = "Mònito";

export const LazyBuilder = (lazyImport: Promise<{ default: () => JSX.Element }>) =>
	(dispatch: React.Dispatch<any>) =>
		React.lazy(() => {
			dispatch(setLoadingActionBuilder(true));
			return lazyImport
				.then(x => {
					dispatch(setLoadingActionBuilder(false));
					return x;
				});
		});