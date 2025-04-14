import forms from '@tailwindcss/forms';

export default {
    content: ['./Views/**/*.cshtml', './ViewComponents/**/*.cshtml', './Pages/**/*.cshtml', './wwwroot/js/**/*.js'],
    plugins: [
        forms,
    ],
    theme: {
        extend: {},
    },
};