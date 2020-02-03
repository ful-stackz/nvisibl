const tailwindcss = require('tailwindcss');

const purgecss = require('@fullhuman/postcss-purgecss')({
    content: [
        './src/**/*.svelte',
        './public/**/*.html',
    ],
    defaultExtractor: (content) => content.match(/[\w-/:]+(?<!:)/g) || [],
});

module.exports = {
    plugins: [
        tailwindcss('./tailwind.config.js'),
        ...(process.env.NODE_ENV === 'production' ? [purgecss] : []),
    ],
};
