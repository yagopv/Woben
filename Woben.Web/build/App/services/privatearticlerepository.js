define(["services/repository"],function(e){function t(e,t,r,a){return new n(e,t,r,a)}var n=function(){var t=function(t,n,r,a){function i(e){return t.manager().executeQuery(e.using(a||breeze.FetchStrategy.FromServer)).then(function(e){return e.results})}e.getCtor.call(this,t,n,r,a),this.all=function(){var e=breeze.EntityQuery.from(r).orderBy("createdDate desc").expand("Tags");return i(e)}};return t.prototype=e.create(),t}();return{create:t}});