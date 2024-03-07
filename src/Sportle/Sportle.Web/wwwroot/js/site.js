// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('service-worker.js').then(function () { console.log('Service worker registered'); });;
};