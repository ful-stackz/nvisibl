import axios from 'axios';

export default class Api {
    private readonly _baseAddress: string;

    constructor(baseAddress: string) {
        if (!baseAddress) throw new Error('Base API address is empty or missing.');
        this._baseAddress = baseAddress.endsWith('/') ? baseAddress : `${baseAddress}/`;
    }

    public get(endpoint: string, query: any, authToken: string) {
        if (!endpoint) throw new Error('Invalid endpoint.');
        const url
            = this._baseAddress
            + endpoint
            + Api.stringifyQuery(query);
        const headers = { ...Api.makeAuthHeader(authToken) };
        return new Promise((resolve, reject) => {
            axios.get(url, { headers })
                .then(resolve)
                .catch(reject);
        });
    }

    public post(endpoint: string, data: any, authToken?: string) {
        if (!endpoint) throw new Error('Invalid endpoint.');
        const url = this._baseAddress + endpoint;
        const headers = { ...Api.makeAuthHeader(authToken) };
        return new Promise((resolve, reject) => {
            axios.post(url, data, { headers })
                .then(resolve)
                .catch(reject);
        });
    }

    private static makeAuthHeader(authToken: string): { authorization: string } | { } {
        return authToken
            ? { authorization: `Bearer ${authToken}` }
            : { };
    }

    private static stringifyQuery(query: any): string {
        if (!query) return '';
        // eslint-disable-next-line prefer-template
        return '?' + Object.entries<string|number|boolean>(query)
            .filter(([, value]) => value && value.toString().length > 0)
            .map(([key, value]) => `${key}=${value}`)
            .join('&');
    }
}
