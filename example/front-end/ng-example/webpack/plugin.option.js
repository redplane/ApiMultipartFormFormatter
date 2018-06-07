const CleanObsoleteChunks = require('webpack-clean-obsolete-chunks');
const ngAnnotatePlugin = require('ng-annotate-webpack-plugin');
const BrowserSyncPlugin = require('browser-sync-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

exports = module.exports = {

    //#region Methods

    get: (bProductionMode, paths) => {

        // Plugins list.
        let plugins = [];

        //#region Clean plugin

        // List of directories to be cleaned.
        const oCleanedItems = [paths.dist];
        const pCleanOption = {
            // Absolute path to your webpack root folder (paths appended to this)
            // Default: root of your package
            root: paths.root,

            // Write logs to console.
            verbose: true,

            // Use boolean "true" to test/emulate delete. (will not remove files).
            // Default: false - remove files
            dry: false,

            // If true, remove files on recompile.
            // Default: false
            watch: false,

            // Instead of removing whole path recursively,
            // remove all path's content with exclusion of provided immediate children.
            // Good for not removing shared files from build directories.
            exclude: null,

            // allow the plugin to clean folders outside of the webpack root.
            // Default: false - don't allow clean folder outside of the webpack root
            allowExternal: false
        };

        if (oCleanedItems.length > 0)
            plugins.push(new CleanWebpackPlugin(oCleanedItems, pCleanOption));

        //#endregion

        //#region Clean obsolete chunks

        plugins.push(new CleanObsoleteChunks({verbose: true}));

        //#endregion

        //#region Copy plugin

        // Items list.
        const oSourceItems = ['assets'];
        let oCopiedItems = [];
        for (let item of oSourceItems){
            oCopiedItems.push({
                from: path.resolve(paths.app, item),
                to: path.resolve(paths.dist, item)
            });
        }

        if (oCopiedItems.length > 0)
            plugins.push(new CopyWebpackPlugin(oCopiedItems));

        //#endregion

        //#region Provide plugin

        // Using bluebird promise instead of native promise.
        plugins.push(new webpack.ProvidePlugin({
            Promise: 'bluebird',
            moment: 'moment',
            jQuery: 'jquery',
            $: 'jquery',
            Rx: 'rxjs'
        }));

        //#region Html plugin

        //Automatically inject chunks into html files.
        plugins.push(new HtmlWebpackPlugin({
            template: path.resolve(paths.source, 'index.html'),
            chunksSortMode: 'dependency'
        }));

        //#endregion

        //#endregion

        if (bProductionMode){
            // Annotate plugin.
            plugins.push(new ngAnnotatePlugin({add: true}));

        } else {

            //#region Browser sync plugin

            // Require original index file.
            let browserSyncPlugin = new BrowserSyncPlugin({
                // browse to http://localhost:3000/ during development,
                // ./public directory is being served
                host: 'localhost',
                port: 8000,
                files: [
                    path.resolve(paths.source, 'index.html')
                ],
                server: {
                    baseDir: [
                        paths.dist
                    ]
                }
            });

            // Push plugins into list.
            plugins.push(browserSyncPlugin);

            //#endregion
        }

        return plugins;

    }

    //#endregion
};