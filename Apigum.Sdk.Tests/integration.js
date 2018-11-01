var obj={};

function setElements(event) {
    obj.OrganizationName = "REPLACE EVENT TOKEN";
}

function template() {
    return `{
  "OrganizationName": "${obj.OrganizationName}"
}`;
}

module.exports = function (context, events) {

    let actions = [];

    for (let event of events.body) {
        setElements(event);
        actions.push(template());
    }

    context.res = {
        body: actions
    };

    context.done();
};