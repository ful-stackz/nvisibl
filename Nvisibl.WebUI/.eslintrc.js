module.exports = {
    env: {
        browser: true,
        es6: true,
    },
    extends: [
        'airbnb-base',
    ],
    globals: {
        Atomics: 'readonly',
        SharedArrayBuffer: 'readonly',
    },
    parser: '@typescript-eslint/parser',
    parserOptions: {
        ecmaVersion: 2018,
        sourceType: 'module',
    },
    settings: {
        'import/resolver': {
            'node': {
                'extensions': [ '.js', '.ts' ],
            },
        },
    },
    plugins: [
        '@typescript-eslint',
    ],
    rules: {
        'linebreak-style': 0,
        'indent': [ 'error', 4, { 'SwitchCase': 1 } ],
        'import/extensions': [ 'error', 'ignorePackages', { 'js': 'never', 'ts': 'never' } ],
        'no-underscore-dangle': [ 'error', { 'allowAfterThis': true } ],
        'operator-linebreak': [ 'error', 'before' ],
        'no-unused-vars': 'off',
        '@typescript-eslint/no-unused-vars': [
            'error',
            {
                'vars': 'all',
                'args': 'after-used',
                'ignoreRestSiblings': false,
            }
        ],
    },
};
