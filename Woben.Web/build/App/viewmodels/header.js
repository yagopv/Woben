define(["plugins/router","services/appsecurity","services/errorhandler","services/utils"],function(e,t,r){var n={router:e,appsecurity:t,logout:function(){var r=this;t.logout().done(function(){t.clearAuthInfo(),window.location=e.activeInstruction().config.authorize?"/account/login":"/home/index"}).fail(r.handlevalidationerrors)}};return r.includeIn(n),n});