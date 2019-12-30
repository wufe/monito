import * as React from 'react';
import './input-form.scss';
import { JobRequestFormFields, JobRequestHTTPMethod } from '~/types/job';

type Props = {
    onSubmit: (fields: JobRequestFormFields) => any;
    disabled: boolean;
}

export const InputForm = (props: React.PropsWithChildren<Props>) => {

    const [advancedSettingsVisibility, setAdvancedSettingsVisibility] = React.useState(false);

    const [fields, setFields] = React.useState<JobRequestFormFields>({
        links: '',
        method: JobRequestHTTPMethod.HEAD,
        redirects: 5,
        threads: 4,
        timeout: 10000,
        userAgent: navigator.userAgent
    });

    const minRedirects    = 0;
    const maxRedirects    = 10;
    const minThreads      = 1;
    const maxThreads      = 4;
    const minTimeout      = 1000;
    const maxTimeout      = 20000;

    const setField =
        (field: keyof JobRequestFormFields) =>
        (value: any) => {
            switch (field) {
                case 'redirects':
                case 'threads':
                case 'timeout':
                    value = +value;
                    if (isNaN(value))
                        value = fields[field];
                    break;
            }
            switch (field) {
                case 'redirects':
                    value = Math.min(maxRedirects, value);
                    value = Math.max(minRedirects, value);
                    break;
                case 'timeout':
                    value = Math.min(maxTimeout, value);
                    value = Math.max(minTimeout, value);
                    break;
                case 'threads':
                    value = Math.min(maxThreads, value);
                    value = Math.max(minThreads, value);
                    break;
            }
            setFields({
                ...fields,
                [field]: value
            });
        }

    // TODO: Check whether there's been an upload before checking links
    // TODO: Display a proper error message
    const trySubmit = () => {
        if (!fields.links.trim())
            return alert('Paste some link');
        props.onSubmit(fields)
    };

    return <div className="input-form__component">
        <div className="__content">
            <textarea
                disabled={props.disabled}
                className="__textarea"
                placeholder="Insert your links here.."
                onChange={e => setField('links')(e.target.value)}
                value={fields.links}></textarea>
            <div className="__bottom-row">
                <div className="__settings-container">
                    <div className={`__settings-content ${advancedSettingsVisibility ? '--visible' : '--hidden'}`}>
                        <div data-input-type="ua" className="__input-container">
                            <label>User Agent</label>
                            <input
                                disabled={props.disabled}
                                type="text"
                                onChange={e => setField('userAgent')(e.target.value)}
                                value={fields.userAgent} />
                        </div>
                        <div data-input-type="method" className="__input-container">
                            <label>HTTP Method</label>
                            <select
                                disabled={props.disabled}
                                onChange={e => setField('method')(e.target.value)}
                                value={fields.method} >
                                {Object.keys(JobRequestHTTPMethod)
                                    .map(m => <option key={m} value={m}>{m}</option>)}
                            </select>
                        </div>
                        <div data-input-type="timeout" className="__input-container">
                            <label>Timeout (ms)</label>
                            <input
                                disabled={props.disabled}
                                step={500}
                                type="number"
                                onChange={e => setField('timeout')(e.target.value)}
                                value={fields.timeout} />
                        </div>
                        <div data-input-type="threads" className="__input-container">
                            <label>Threads</label>
                            <input
                                disabled={props.disabled}
                                type="number"
                                onChange={e => setField('threads')(e.target.value)}
                                value={fields.threads} />
                        </div>
                        <div data-input-type="redirects" className="__input-container">
                            <label>Max redirects</label>
                            <input
                                disabled={props.disabled}
                                type="number"
                                onChange={e => setField('redirects')(e.target.value)}
                                value={fields.redirects} />
                        </div>
                    </div>
                    <div className="__settings-toggler"
                        onClick={() => setAdvancedSettingsVisibility(!advancedSettingsVisibility)}>
                        {advancedSettingsVisibility ? 'Hide' : 'Show'} advanced settings
                    </div>
                </div>
                <div className="__actions-container">
                    <button
                        disabled={props.disabled}
                        className="--success"
                        onClick={trySubmit}>Start</button>
                </div>
            </div>
        </div>
    </div>;
}