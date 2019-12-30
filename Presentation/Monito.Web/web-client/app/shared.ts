import * as React from "react";
import { SetLoadingActionBuilder } from "~/state/action";

export const APPLICATION_NAME = "Mònito";

export const LazyBuilder = (lazyImport: Promise<{ default: () => JSX.Element }>) =>
	(dispatch: React.Dispatch<any>) =>
		React.lazy(() => {
			dispatch(SetLoadingActionBuilder(true));
			return lazyImport
				.then(x => {
					dispatch(SetLoadingActionBuilder(false));
					return x;
				});
		});