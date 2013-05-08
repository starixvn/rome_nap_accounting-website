<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmember_master">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    {lbl:closed}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@closed" />
                </td>
                <td class="colW1">
                    {lbl:closedMultiple}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@closedmultiple" />
                </td>
                <td class="colW1">
                  {lbl:suspended}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@suspended" />
                </td>
            </tr>

            <tr>
                
                <td class="colW1">
                    {lbl:suspendedMultiple}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@suspendedmultiple" />
                </td>
                <td class="colW1">
                  {lbl:language}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@language" />
                </td>
                <td class="colW1">
                  {lbl:subAccName}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@subaccname" />
                </td>

            </tr>

            <tr>              
                <td class="colW1">
                    {lbl:ip}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>
